import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ProductDto, ProductsClient } from '../../rentasgt-api';
import { PageInfo } from '../../models/PageInfo';

@Component({
  selector: 'app-products',
  templateUrl: './products.component.html',
  styleUrls: ['./products.component.css']
})
export class ProductsComponent implements OnInit {

  public PAGE_SIZE = 15;
  public DEFAULT_PAGE_NUMBER = 1;

  public products: ProductDto[] = [];
  public pageInfo: PageInfo = null;
  public loadingProducts = false;

  public PRODUCT_STATES = [
    'Incompleto',
    'Disponible',
    'Ocupado'
  ];

  constructor(
    private productsClient: ProductsClient,
    private router: Router,
  ) { }

  ngOnInit(): void {
    this.loadProducts();
  }

  private loadProducts(pageSize = this.PAGE_SIZE, pageNumber = this.DEFAULT_PAGE_NUMBER): void {
    this.loadingProducts = true;
    this.productsClient.getProductsOfOwner(pageSize, pageNumber)
      .subscribe((res) => {
        this.products = res.items;
        this.pageInfo = new PageInfo(res.currentPage, res.totalPages, res.pageSize, res.totalCount);
        this.loadingProducts = false;
      }, error => {
        this.loadingProducts = false;
        console.error(error);
      });
  }

  public onPageChange($event): void {
    console.log($event);
  }

}
