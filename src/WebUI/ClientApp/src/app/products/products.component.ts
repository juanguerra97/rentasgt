import { Component, Inject, OnInit, TemplateRef } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { BsModalRef, BsModalService } from 'ngx-bootstrap';
import { API_BASE_URL, CategoriesClient, CategoryDto, ProductDto, ProductsClient, RatingToProductDto, RatingToProductsClient } from '../rentasgt-api';
import { LocationInfo } from '../models/LocationInfo';
import { PageInfo } from '../models/PageInfo';
import { RateProductComponent } from '../app-common/rate-product/rate-product.component';
import { MatDialog } from '@angular/material/dialog';

@Component({
  selector: 'app-products',
  templateUrl: './products.component.html',
  styleUrls: ['./products.component.css']
})
export class ProductsComponent implements OnInit {

  public PAGE_SIZE = 10;
  public DEFAULT_PAGE_NUMBER = 1;

  public searchText = '';
  public category: CategoryDto = null;
  public location: LocationInfo = {
    formattedAddress: null,
    city: null,
    state: null,
    country: null,
    longitude: null,
    latitude: null
  };
  public distance: number = 10;
  public categories: CategoryDto[] = [];
  public locationModalRef: BsModalRef|null = null;

  public searchingProducts = false;
  public products: ProductDto[] = [];
  public pageInfo: PageInfo = null;

  public filter: ProductFilter = {};

  constructor(
    private productsClient: ProductsClient,
    private categoriesClient: CategoriesClient,
    private ratingToProductsClient: RatingToProductsClient,
    private activatedRoute: ActivatedRoute,
    private bsModalService: BsModalService,
    private matDialog: MatDialog,
    @Inject(API_BASE_URL) public baseUrl?: string,
  ) { }

  ngOnInit(): void {
    this.activatedRoute.paramMap.subscribe((params) => {
      this.searchingProducts = true;
      const searchParam = params.get('s');
      let name = undefined;
      if (searchParam && searchParam.trim().length > 0) {
        this.searchText = searchParam.trim();
        name = this.searchText;
      }

        this.productsClient.get(this.PAGE_SIZE, this.DEFAULT_PAGE_NUMBER, name,
          undefined, undefined, undefined, undefined, undefined, undefined)
          .subscribe((res) => {
            this.products = res.items;
            this.pageInfo = new PageInfo(res.currentPage, res.totalPages, res.pageSize, res.totalCount);
            this.searchingProducts = false;
          }, error => {
            this.searchingProducts = false;
            console.error(error);
          });
    }, console.error);
    this.loadCategories();
    setTimeout(() => this.checkIfThereIsPendingProductRating(), 1500);
  }

  private loadProducts(pageSize: number = this.PAGE_SIZE, pageNumber = this.DEFAULT_PAGE_NUMBER): void {
    this.productsClient.get(pageSize, pageNumber, this.filter.name, this.filter.category, this.filter.longitude,
      this.filter.latitude, this.filter.distance, this.filter.city, this.filter.state)
      .subscribe((res) => {
        this.products = res.items;
        this.pageInfo = new PageInfo(res.currentPage, res.totalPages, res.pageSize, res.totalCount);
        this.searchingProducts = false;
      }, error => {
        this.searchingProducts = false;
        console.error(error);
      });
  }

  private loadCategories(): void {
    this.categoriesClient.get(70, 1)
      .subscribe((res) => {
        this.categories = res.items;
      }, console.error);
  }

  public onFiltrar(): void {
    this.searchingProducts = true;

    if (!this.searchText || this.filter.name.length === 0) {
      this.filter.name = undefined;
    } else {
      this.filter.name = this.searchText.trim();
    }
    this.filter.latitude = this.location.latitude !== null ? this.location.latitude : undefined;
    this.filter.longitude = this.location.longitude !== null ? this.location.longitude : undefined;
    this.filter.category = this.category !== null ? this.category.id : undefined;
    this.filter.distance = this.location.latitude !== null && this.location.longitude !== null ? this.distance : undefined;

    this.loadProducts();
  }

  public onSelectLocation(template: TemplateRef<any>): void {
    this.locationModalRef = this.bsModalService.show(template, { ignoreBackdropClick: true });
  }

  public onLocationSelected(location: LocationInfo): void {
    this.location = location;
    this.locationModalRef.hide();
  }

  public clearLocation(): void {
    this.location = {
      formattedAddress: null,
      city: null,
      state: null,
      country: null,
      longitude: null,
      latitude: null
    };
  }

  public imgLoaded($event) {
    const img: HTMLImageElement = $event.target;
    const ratio = img.width / img.height;
    if (ratio >= 1.0) {
      img.parentElement.classList.remove('img-wrapper-h');
      img.parentElement.classList.add('img-wrapper-w');
    } else {
      img.parentElement.classList.remove('img-wrapper-w');
      img.parentElement.classList.add('img-wrapper-h');
    }
  }

  private checkIfThereIsPendingProductRating(): void {
    this.ratingToProductsClient.getPending()
      .subscribe((res) => {
        this.askUserForProductRating(res);
      }, console.error);
  }

  private askUserForProductRating(pendingRating: RatingToProductDto): void {
    const ref = this.matDialog.open(RateProductComponent, {
      width: '350px',
      panelClass: 'mat-dialog-panel',
      data: pendingRating
    });
  }

}

class ProductFilter {
  category?: number;
  name?: string;
  latitude?: number;
  longitude?: number;
  city?: string;
  state?: string;
  distance?: number;
}
