using ProductService.Models;
using System.Linq;
using System.Web.OData;

namespace ProductService.Controllers
{
    public class SuppliersController : ODataController
    {
        ProductsContext db = new ProductsContext();

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        ////////////

        









        ////////////


        [EnableQuery]
        public IQueryable<Product> GetProducts([FromODataUri] int key)
        {
            return db.Suppliers.Where(m => m.Id.Equals(key)).SelectMany(m => m.Products);
        }


        public async Task<IHttpActionResult> DeleteRef([FromODataUri] int key,
        [FromODataUri] string relatedKey, string navigationProperty)
        {
            var supplier = await db.Suppliers.SingleOrDefaultAsync(p => p.Id == key);
            if (supplier == null)
            {
                return StatusCode(HttpStatusCode.NotFound);
            }

            switch (navigationProperty)
            {
                case "Products":
                    var productId = Convert.ToInt32(relatedKey);
                    var product = await db.Products.SingleOrDefaultAsync(p => p.Id == productId);

                    if (product == null)
                    {
                        return NotFound();
                    }
                    product.Supplier = null;
                    break;
                default:
                    return StatusCode(HttpStatusCode.NotImplemented);

            }
            await db.SaveChangesAsync();

            return StatusCode(HttpStatusCode.NoContent);
        }


    }
}