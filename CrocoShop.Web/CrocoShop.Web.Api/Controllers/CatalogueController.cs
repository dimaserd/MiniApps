using Croco.Core.Contract.Models;
using Microsoft.AspNetCore.Mvc;
using Prd.Contract.Models.Catalogue;
using Prd.Contract.Models.ProductCharacteristics;
using Prd.Contract.Models.Products.Search;
using Prd.Logic.Services.Catalogues;
using Prd.Logic.Services.Products;
using System;
using System.Threading.Tasks;

namespace CrocoShop.Main.Api.Controllers.MainApp
{
    /// <inheritdoc />
    /// <summary>
    /// Предоставляет методы для работы с товарным каталогом
    /// </summary>
    [Route("Api/Catalogue")]
    public class CatalogueController : ControllerBase
    {
        CatalogueWorker CatalogueService { get; }

        CatalogueCharacteristicsSearchService CharacteristicsSearchService { get; }

        ProductsShowService ProductsShowService { get; }
        CatalogueFilterBuildService CatalogueFilterBuildService { get; }
        CatalogueQueryByViewModelService CatalogueQueryByViewModelService { get; }

        /// <inheritdoc />
        public CatalogueController(CatalogueWorker catalogueService, 
            CatalogueCharacteristicsSearchService characteristicsSearchService,
            CatalogueFilterBuildService catalogueFilterBuildService,
            ProductsShowService productsShowService)
        {
            CatalogueService = catalogueService;
            CharacteristicsSearchService = characteristicsSearchService;
            CatalogueFilterBuildService = catalogueFilterBuildService;
            ProductsShowService = productsShowService;
        }


        /// <summary>
        /// Показ товара
        /// </summary>
        /// <param name="productAlias"></param>
        /// <returns></returns>
        [HttpPost("Show")]
        public Task<BaseApiResponse<ProductInCatalogueModel>> ShowProduct([FromQuery]string productAlias)
            => ProductsShowService.ShowProductByAliasAsync(productAlias);

        /// <summary>
        /// Получение товаров в каталоге выбранных по модели
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("Products")]
        public Task<ProductsShowModel> ProductsInCatalogue([FromBody]ProductsSelectionModelBase model)
            => CatalogueService.GetProductsInCatalogueBaseAsync(model);

        /// <summary>
        /// Получить модель для построения фильтра товаров
        /// </summary>
        /// <returns></returns>
        [HttpPost("Filter/GetModel")]
        public Task<CatalogueFilterBuildModel> GetCatalogueFilterModel()
        {
            return CatalogueFilterBuildService.GetCatalogueFilterModelCached(TimeSpan.FromMinutes(2));
        }

        /// <summary>
        /// Получение товаров в каталоге выбранных по модели
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("Products/View")]
        public Task<ProductsShowModel> ProductsInCatalogueByViewModel([FromBody]SelectionViewModelBase model)
        {
            return CatalogueQueryByViewModelService.ProductsInCatalogueByViewModel(model, true);
        }

        /// <summary>
        /// Получить все значения товарных характеристик со счетчиками
        /// </summary>
        /// <returns></returns>
        [HttpPost("Characteristics/Counts")]
        public Task<AllProductCharacteristicWithUniqueValuesWithCounts> GetAllProductCharacteristicValues()
        {
            return CharacteristicsSearchService.GetAllProductCharacteristicWithValuesCount();
        }

        /// <summary>
        /// Получить все значения товарных характеристик со счетчиками
        /// </summary>
        /// <returns></returns>
        [HttpPost("Characteristics/ProductValues")]
        public Task<AllProductCharacteristicsWithAllProductValues> GetAllProductCharacteristicWithAllProductValues()
        {
            return CharacteristicsSearchService.GetAllProductCharacteristicWithAllProductValues();
        }
    }
}