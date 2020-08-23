import { Component, OnInit, TemplateRef } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { BsModalRef, BsModalService } from 'ngx-bootstrap';
import { CategoriesClient, CategoryDto, ProductDto, ProductsClient} from '../rentasgt-api';
import { LocationInfo } from '../models/LocationInfo';
import { PageInfo } from '../models/PageInfo';

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
  public categories: CategoryDto[] = [];
  public locationModalRef: BsModalRef|null = null;

  public searchingProducts = false;
  public products: ProductDto[] = [];
  public pageInfo: PageInfo = null;

  public filter: ProductFilter = {};

  constructor(
    private productsClient: ProductsClient,
    private categoriesClient: CategoriesClient,
    private activatedRoute: ActivatedRoute,
    private bsModalService: BsModalService,
  ) { }

  ngOnInit(): void {
    this.activatedRoute.paramMap.subscribe((params) => {
      this.searchText = params.get('s');
      if (this.searchText !== null && this.searchText !== undefined) {
        this.searchText = this.searchText.trim();
        let name = this.searchText;
        if (this.searchText.length === 0) {
          name = undefined;
        }
        this.searchingProducts = true;
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
      }
    }, console.error);
    this.loadCategories();
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
    this.filter.name = this.searchText.trim();
    if (this.filter.name.length === 0) {
      this.filter.name = undefined;
    }
    this.filter.latitude = this.location.latitude !== null ? this.location.latitude : undefined;
    this.filter.longitude = this.location.longitude !== null ? this.location.longitude : undefined;
    this.filter.category = this.category !== null ? this.category.id : undefined;

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
