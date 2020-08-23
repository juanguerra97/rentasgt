import { Component, OnInit, TemplateRef } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { BsModalRef, BsModalService } from 'ngx-bootstrap';
import {CategoriesClient, CategoryDto} from '../rentasgt-api';
import { LocationInfo } from '../models/LocationInfo';

@Component({
  selector: 'app-products',
  templateUrl: './products.component.html',
  styleUrls: ['./products.component.css']
})
export class ProductsComponent implements OnInit {

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

  constructor(
    private categoriesClient: CategoriesClient,
    private activatedRoute: ActivatedRoute,
    private bsModalService: BsModalService,
  ) { }

  ngOnInit(): void {
    this.activatedRoute.paramMap.subscribe((params) => {
      this.searchText = params.get('s');
    }, console.error);
    this.loadCategories();
  }

  private loadCategories(): void {
    this.categoriesClient.get(70, 1)
      .subscribe((res) => {
        this.categories = res.items;
      }, console.error);
  }

  public onFiltrar(): void {
    this.searchingProducts = true;
    let name = this.searchText.trim();
    if (name.length === 0) {
      name = undefined;
    }
    const latitude = this.location.latitude !== null ? this.location.latitude : undefined;
    const longitude = this.location.longitude !== null ? this.location.longitude : undefined;
    const category = this.category !== null ? this.category.id : undefined;
    // TODO: make call to retrieve products
    this.searchingProducts = false;
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
