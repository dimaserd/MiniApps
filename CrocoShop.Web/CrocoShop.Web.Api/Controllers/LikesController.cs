using System.Threading.Tasks;
using Croco.Core.Contract.Models;
using Microsoft.AspNetCore.Mvc;
using Prd.Contract.Models.ProductLikes;
using Prd.Logic.Services.Likes;

namespace CrocoShop.Main.Api.Controllers.MainApp
{
    /// <inheritdoc />
    /// <summary>
    /// Апи-контроллер предоставляющий методы для работы с лайками, которые поставил пользователь товарам
    /// </summary>
    [Route("Api/Likes")]
    public class LikesController : ControllerBase
    {
        /// <inheritdoc />
        public LikesController(LikesService likesService)
        {
            LikesService = likesService;
        }

        LikesService LikesService { get; }

        /// <summary>
        /// Добавления лайка товару
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("LikeProduct")]
        public Task<BaseApiResponse> LikeProduct([FromBody]AddOrDeleteProductLike model) => LikesService.LikeProduct(model);

        /// <summary>
        /// Удаление лайка из товара
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("DeleteProductLike")]
        public Task<BaseApiResponse> DeleteProductLike([FromBody]AddOrDeleteProductLike model)
            => LikesService.DeleteProductLike(model);

        /// <summary>
        /// Получает кол-во понравившихся товаров текущему пользователю
        /// </summary>
        /// <returns></returns>
        [HttpGet("Count")]
        public Task<int> GetLikesCount() => LikesService.GetMyLikesCountAsync();
    }
}