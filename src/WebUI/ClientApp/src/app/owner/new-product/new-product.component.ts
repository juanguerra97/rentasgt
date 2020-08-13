import {Component, OnInit, TemplateRef} from '@angular/core';
import { imgBlobToBase64 } from '../../utils';
import {BsModalRef, BsModalService} from 'ngx-bootstrap';

@Component({
  selector: 'app-new-product',
  templateUrl: './new-product.component.html',
  styleUrls: ['./new-product.component.css']
})
export class NewProductComponent implements OnInit {

  public uploadedImages: Img[] = [];
  public currentImg: Img = null;

  public cropImgModalRef: BsModalRef|null = null;

  constructor(
    private bsModalService: BsModalService,
  ) { }

  ngOnInit(): void {
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

}

class Img {
  file: File;
  origContent: string|ArrayBuffer = null;
  contentCropped: Blob = null;
  imgCropped: string|ArrayBuffer = null;
}
