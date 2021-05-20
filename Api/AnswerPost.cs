using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

namespace Api
{
    public class AnswerPost
    {
        [FunctionName("AnswerPost")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "answer")] HttpRequest req,
            ILogger log)
        {
            var seedString = req.Query.TryGetValue("seed", out var values) ? values.First() : "1";
            var seed = int.TryParse(seedString, out var n) ? n : 1;

            var random = new Random(seed);
            const int numOfOptions = 4;
            const int numOfQuestions = 10;
            var correct = Enumerable.Range(0, numOfQuestions).Select(_ => random.Next(0, numOfOptions) + 1).ToArray();

            var body = await new StreamReader(req.Body).ReadToEndAsync();
            var answers = JsonSerializer.Deserialize<int[]>(body);

            var correctCount = correct
                .Zip(answers, (c, a) => c == a)
                .Count(right => right);

            return new OkObjectResult(correctCount);
        }
    }
}
