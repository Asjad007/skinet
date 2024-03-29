using API.Dtos;
using API.Entities;
using API.Errors;
using AutoMapper;
using Core.Interfaces;
using Core.Specifications;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class ProductsController : BaseApiController
    {
        private readonly IGenericRepository<Product> _productRepo;
        private readonly IGenericRepository<ProductBrand> _productBrandRepo;
        private readonly IGenericRepository<ProductType> _productTypeRepo;
        private readonly IMapper _mapper;

         public ProductsController(
         IGenericRepository<Product> productRepo, IGenericRepository<ProductBrand> productBrandRepo, IGenericRepository<ProductType> productTypeRepo, IMapper mapper)
         {
             _mapper = mapper;
            _productTypeRepo = productTypeRepo;
            _productBrandRepo = productBrandRepo;
            _productRepo = productRepo;
            
         }
        [HttpGet]
       public async Task<ActionResult<IReadOnlyList<ProductToReturnDto>>> GetProducts(){
       var spec = new ProductWithTypesAndBrandsSpecification();
       var products = await _productRepo.ListAsync(spec);
       return Ok(_mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDto>>(products));
       }


        [HttpGet("{Id}")]
        [HttpGet("{Id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse),  StatusCodes.Status404NotFound)]
       public async Task<ActionResult<ProductToReturnDto>> GetProduct(int Id){
            var spec = new ProductWithTypesAndBrandsSpecification(Id);
           var product = await _productRepo.GetEntityWithSpec(spec);
           if(product == null) return NotFound(new ApiResponse(404));
            return _mapper.Map<Product, ProductToReturnDto>(product);
       }

       [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetProductBrands(){

            return Ok(await _productBrandRepo.ListAllAsync());
       }

       [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<ProductType>>> GetProductTypes(){

            return Ok(await _productTypeRepo.ListAllAsync());
       }
    }


}