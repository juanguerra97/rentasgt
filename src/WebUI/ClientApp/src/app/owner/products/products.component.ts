import { Component, OnInit } from '@angular/core';
import { ProductDto, ProductsClient } from '../../rentasgt-api';

@Component({
  selector: 'app-products',
  templateUrl: './products.component.html',
  styleUrls: ['./products.component.css']
})
export class ProductsComponent implements OnInit {

  public products: ProductDto[] = [];

  constructor(
    private productsClient: ProductsClient
  ) { }

  ngOnInit(): void {
  }

}
