import { Component, OnInit, TemplateRef } from '@angular/core';
import { BsModalRef, BsModalService } from 'ngx-bootstrap';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import {
  CategoriesClient,
  CategoryDto, CategorySummaryDto,
  PaginatedListResponseOfCategoryDto,
  PicturesClient,
  ProductDto,
  ProductsClient
} from '../../rentasgt-api';
import { LocationInfo } from '../../models/LocationInfo';
import {ActivatedRoute, Router} from '@angular/router';
import { imgBlobToBase64, getErrorsFromResponse } from '../../utils';
import { MessageService } from 'primeng';

@Component({
  selector: 'app-edit-product',
  templateUrl: './edit-product.component.html',
  styleUrls: ['./edit-product.component.css']
})
export class EditProductComponent implements OnInit {

  public product: ProductDto = null;
  public productCategories: CategorySummaryDto[] = [];
  public loadingProduct: boolean = false;
  public removedImages: number[] = [];
  public uploadedImages: Img[] = [];
  public currentImg: Img = null;
  public cropImgModalRef: BsModalRef|null = null;
  public locationModalRef: BsModalRef|null = null;
  public saving = false;

  public editProductForm = new FormGroup({
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
    private activatedRoute: ActivatedRoute,
    private bsModalService: BsModalService,
    private messageService: MessageService,
  ) { }

  ngOnInit(): void {
    this.activatedRoute.paramMap.subscribe(async (params) => {
      const id = Number.parseInt(params.get('id'));
      if (id) {
        this.loadProduct(id);
      } else {
        await this.router.navigate(['/propietario/productos']);
      }
    });
  }

  private loadProduct(productId: number): void {
    this.loadingProduct = true;
    this.productsClient.getById(productId)
      .subscribe((res) => {
        this.product = res;
        this.editProductForm.setValue({
          name: this.product.name,
          otherNames: this.product.otherNames,
          description: this.product.description,
          costPerDay: this.product.costPerDay,
          costPerWeek: this.product.costPerWeek,
          costPerMonth: this.product.costPerMonth,
        });
        for (const pic of this.product.pictures) {
          this.uploadedImages.push({
            id: pic.pictureId,
            imgCropped: pic.pictureContent
          });
        }
        this.location.latitude = this.product.location.latitude;
        this.location.longitude = this.product.location.longitude;
        this.location.state = this.product.location.state;
        this.location.city = this.product.location.city;
        this.location.formattedAddress = `${this.location.city}, ${this.location.state}`;
        this.loadingProduct = false;
        this.loadCategories();
      }, async (error) => {
        await this.router.navigate(['/propietario/productos']);
      });
  }

  private loadCategories(): void {
    this.categoriesClient.get(50, 1)
      .subscribe((res: PaginatedListResponseOfCategoryDto) => {
        this.categories = res.items;
        this.productsClient.getCategories(this.product.id)
          .subscribe((categs) => {
            this.productCategories = categs;
            for (const categ of categs) {
              this.selectedCategories.push(
                this.categories.find(c => c.id === categ.id)
              );
            }
          }, console.error);
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
    const img = this.uploadedImages[index];
    if (img.id) {
      this.removedImages.push(img.id);
    }
    this.uploadedImages.splice(index, 1);
  }

  public onSelectLocation(template: TemplateRef<any>): void {
    this.locationModalRef = this.bsModalService.show(template, { ignoreBackdropClick: true });
  }

  public onLocationSelected(location: LocationInfo): void {
    this.location = location;
    this.locationModalRef.hide();
  }

  public async onSubmitEditProductForm(): Promise<any> {
    this.saving = true;
    const newPictures: number[] = [];
    this.messageService.clear();
    try {
      for (const img of this.uploadedImages) {
       if (!img.id) {
         newPictures.push(await this.picturesClient.uploadPublicPicture({
           data: img.contentCropped,
           fileName: img.file.name,
         }).toPromise());
       }
      }

      const categoriesToRemove: number[] = [];
      for (const categ of this.productCategories) {
        const i = this.selectedCategories.findIndex(cat => cat.id === categ.id);
        if (i < 0) {
          categoriesToRemove.push(categ.id);
        }
      }

      const newCategories: number[] = [];
      for (const categ of this.selectedCategories) {
        const i = this.productCategories.findIndex(cat => cat.id === categ.id);
        if (i < 0) {
          newCategories.push(categ.id);
        }
      }

      const newProduct = Object.assign({ id: this.product.id }, this.editProductForm.value,
        { location: Object.assign({}, this.location)}, { newCategories, categoriesToRemove, newPictures, picturesToRemove: this.removedImages });

      if (newProduct.name === this.product.name) {
        newProduct.name = undefined;
      }
      if (newProduct.otherNames === this.product.otherNames) {
        newProduct.otherNames = undefined;
      }
      if (newProduct.description === this.product.description) {
        newProduct.description = undefined;
      }
      if (newProduct.location.latitud === this.product.location.latitude && newProduct.location.longitude === this.product.location.longitude) {
        newProduct.location = undefined;
      }
      if (newProduct.costPerDay == this.product.costPerDay) {
        newProduct.costPerDay = undefined;
      }
      if (newProduct.costPerMonth === '') {
        newProduct.costPerMonth = undefined;
      }
      if (newProduct.costPerWeek === '') {
        newProduct.costPerWeek = undefined;
      }
      //console.log(newProduct);
      await this.productsClient.update(this.product.id, newProduct).toPromise();

      await this.router.navigate(['/propietario/productos/detalle/', this.product.id]);

    } catch (err) {
      if (err.response) {
        const errors = getErrorsFromResponse(JSON.parse(err.response));
        if (errors.length > 0) {
          this.messageService.add({ severity: 'error', detail: errors[0], closable: true });
        }
      } else {
        console.error(err);
      }
    } finally {
      this.saving = false;
    }
  }

}

class Img {
  id?: number;
  file?: File;
  origContent?: string|ArrayBuffer = null;
  contentCropped?: Blob = null;
  imgCropped: string|ArrayBuffer = null;
}
