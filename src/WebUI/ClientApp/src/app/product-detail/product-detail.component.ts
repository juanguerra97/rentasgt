import { Component, OnInit } from '@angular/core';
import { ProductDto, ProductsClient } from '../rentasgt-api';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-product-detail',
  templateUrl: './product-detail.component.html',
  styleUrls: ['./product-detail.component.css']
})
export class ProductDetailComponent implements OnInit {

  public product: ProductDto = null;
  public loadingProduct = false;
  public notFound = false;

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
    private router: Router,
    private activatedRoute: ActivatedRoute,
  ) { }

  ngOnInit() {
    this.activatedRoute.paramMap.subscribe(async (params) => {
      const id = Number.parseInt(params.get('id'));
      if (!id) {
        await this.router.navigate(['/articulos']);
      } else {
        this.loadProduct(id);
      }
    });
  }

  private loadProduct(productId: number): void {
    this.loadingProduct = true;
    this.productsClient.getById(productId)
      .subscribe((res) => {
        this.product = res;
        this.loadingProduct = false;
    }, error => {
        this.loadingProduct = false;
        this.notFound = error.status == 404;
      });
  }

}
