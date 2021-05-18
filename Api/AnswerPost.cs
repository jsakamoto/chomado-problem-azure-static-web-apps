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
            var body = await new StreamReader(req.Body).ReadToEndAsync();
            var answers = JsonSerializer.Deserialize<int[]>(body);

            var correct = new[] { 1, 1, 4, 3, 3, 4, 2, 1, 3, 2 };
            var correctCount = correct
                .Zip(answers, (c, a) => c == a)
                .Count(right => right);

            return new OkObjectResult(correctCount);
        }
    }
}
