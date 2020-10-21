import { Component, Inject, OnInit } from '@angular/core';
import {API_BASE_URL, ProductDto, ProductsClient, UserProfileDto, UserProfileStatus, UsersClient} from '../../rentasgt-api';
import { PageInfo } from '../../models/PageInfo';

@Component({
  selector: 'app-products',
  templateUrl: './products.component.html',
  styleUrls: ['./products.component.css']
})
export class ProductsComponent implements OnInit {

  public PAGE_SIZE = 10;
  public DEFAULT_PAGE_NUMBER = 1;

  public PROFILE_ACTIVE = UserProfileStatus.Active;
  public currentUser: UserProfileDto = null;
  public loadingUser: boolean = false;

  public products: ProductDto[] = [];
  public pageInfo: PageInfo = null;
  public loadingProducts = true;

  public PRODUCT_STATES = [
    'Incompleto',
    'Disponible',
    'Ocupado'
  ];

  constructor(
    private productsClient: ProductsClient,
    private usersClient: UsersClient,
    @Inject(API_BASE_URL) public baseUrl?: string,
  ) {}

  ngOnInit(): void {
    this.loadCurrentUser();
    this.loadProducts();
  }

  public loadCurrentUser(): void {
    this.loadingUser = true;
    this.usersClient.getUserProfile()
      .subscribe((res) => {
        this.currentUser = res;
        this.loadingUser = false;
      }, error => {
        console.error(error);
        this.loadingUser = false;
      });
  }

  public loadProducts(pageSize = this.PAGE_SIZE, pageNumber = this.DEFAULT_PAGE_NUMBER): void {
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

}
