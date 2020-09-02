import { Component, OnInit, TemplateRef } from '@angular/core';
import { Router } from '@angular/router';
import { imgBlobToBase64 } from '../../utils';
import { BsModalRef, BsModalService } from 'ngx-bootstrap';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { LocationInfo } from '../../models/LocationInfo';
import {
  CategoriesClient,
  CategoryDto,
  PaginatedListResponseOfCategoryDto,
  PicturesClient,
  ProductsClient
} from '../../rentasgt-api';
import { Img } from '../../models/Img';

@Component({
  selector: 'app-new-product',
  templateUrl: './new-product.component.html',
  styleUrls: ['./new-product.component.css']
})
export class NewProductComponent implements OnInit {

  public uploadedImages: Img[] = [];
  public currentImg: Img = null;
  public cropImgModalRef: BsModalRef|null = null;
  public locationModalRef: BsModalRef|null = null;
  public saving = false;

  public newProductForm = new FormGroup({
    name: new FormControl('', [
      Validators.required,
    ]),
    description: new FormControl('', [
      Validators.required,
    ]),
    otherNames: new FormControl('', [
      Validators.required,
    ]),
    costPerDay: new FormControl('', [
      Validators.required,
      Validators.min(1),
      Validators.max(5000),
    ]),
    costPerWeek: new FormControl('', [
      Validators.min(2),
      Validators.max(35000),
    ]),
    costPerMonth: new FormControl('', [
      Validators.min(4),
      Validators.max(155000),
    ]),
  });

  public categories: CategoryDto[] = [];
  public selectedCategories: CategoryDto[] = [];

  public location: LocationInfo = {
    formattedAddress: null,
    state: 'Guatemala',
    country: 'Guatemala',
    city: 'Guatemala',
    longitude: null,
    latitude: null,
  };

  constructor(
    private productsClient: ProductsClient,
    private categoriesClient: CategoriesClient,
    private picturesClient: PicturesClient,
    private router: Router,
    private bsModalService: BsModalService,
  ) { }

  ngOnInit(): void {
    this.loadCategories();
  }

  private loadCategories(): void {
    this.categoriesClient.get(50, 1)
      .subscribe((res: PaginatedListResponseOfCategoryDto) => {
        this.categories = res.items;
      }, console.error);
  }

  public async onImgFileChange(event, template: TemplateRef<any>): Promise<any> {
    event.preventDefault();
    this.currentImg = null;
    const fileElement = event.target;
    if (fileElement.files.length > 0) {
      this.currentImg = new Img();
      this.currentImg.file = fileElement.files[0];
      try {
        this.currentImg.origContent = await imgBlobToBase64(this.currentImg.file);
        this.cropImgModalRef = this.bsModalService.show(template, { ignoreBackdropClick: true });
      } catch (error) {
        this.currentImg = null;
      }
    }
  }

  public async onImageCropped(img: Blob): Promise<any> {
    this.currentImg.contentCropped = img;
    try {
      this.currentImg.imgCropped = await imgBlobToBase64(img);
      this.uploadedImages.push(this.currentImg);
      // TODO: send image to server
    } catch (error) { }
    this.currentImg = null;
    this.cropImgModalRef.hide();
  }

  public onUploadImgCancelled(): void {
    this.cropImgModalRef.hide();
    this.currentImg = null;
  }

  public removeImg(index: number): void {
    this.uploadedImages.splice(index, 1);
  }

  public onSelectLocation(template: TemplateRef<any>): void {
    this.locationModalRef = this.bsModalService.show(template, { ignoreBackdropClick: true });
  }

  public onLocationSelected(location: LocationInfo): void {
    this.location = location;
    this.locationModalRef.hide();
  }

  public async onSubmitNewProductForm(): Promise<any> {
    this.saving = true;
    const pics: number[] = [];
    try {
      for (const img of this.uploadedImages) {
        pics.push(await this.picturesClient.uploadPublicPicture({
          data: img.contentCropped,
          fileName: img.file.name,
        }).toPromise());
      }

      const newProduct = Object.assign({}, this.newProductForm.value,
        { pictures: pics }, { categories: this.selectedCategories.map(c => c.id)},
        { location: Object.assign({}, this.location)});
      if (newProduct.costPerMonth === '') {
        delete newProduct.costPerMonth;
      }
      if (newProduct.costPerWeek === '') {
        delete newProduct.costPerWeek;
      }
      const idNewProd = await this.productsClient.create(newProduct).toPromise();

      this.uploadedImages = [];
      this.selectedCategories = [];
      this.location.formattedAddress = null;
      this.location.latitude = null;
      this.location.longitude = null;
      this.newProductForm.reset();

      await this.router.navigate(['/propietario/productos/detalle/', idNewProd]);

    } catch (err) {
      console.error(err);
    } finally {
      this.saving = false;
    }
  }

}
