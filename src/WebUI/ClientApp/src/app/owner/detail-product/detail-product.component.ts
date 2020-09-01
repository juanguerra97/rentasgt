import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ProductDto, ProductsClient } from '../../rentasgt-api';

@Component({
  selector: 'app-detail-product',
  templateUrl: './detail-product.component.html',
  styleUrls: ['./detail-product.component.css']
})
export class DetailProductComponent implements OnInit {

  public product: ProductDto = null;
  public loadingProduct = false;

  responsiveOptions: any[] = [
    {
      breakpoint: '1024px',
      numVisible: 5
    },
    {
      breakpoint: '768px',
      numVisible: 3
    },
    {
      breakpoint: '560px',
      numVisible: 1
    }
  ];

  constructor(
    private productsClient: ProductsClient,
    private route: ActivatedRoute,
    private router: Router,
  ) { }

  ngOnInit(): void {
    this.route.paramMap.subscribe(params => {
      const productId = Number.parseInt(params.get('id'));
      if (!productId) {
        this.router.navigate(['/propietario/productos']);
      } else {
        this.loadProduct(productId);
      }
    });
  }

  private loadProduct(productId: number): void {
    this.loadingProduct = true;
    this.productsClient.getById(productId).subscribe((res) => {
      this.product = res;
      this.loadingProduct = false;
    }, error => {
      this.loadingProduct = false;
      console.error(error);
      this.router.navigate(['/propietario/productos']);
    });
  }

}
