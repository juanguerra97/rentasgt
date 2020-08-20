import { Component, EventEmitter, Input, Output, OnInit, AfterViewInit } from '@angular/core';
import * as Croppie from 'croppie';

@Component({
  selector: 'app-img-cropper',
  templateUrl: './img-cropper.component.html',
  styleUrls: ['./img-cropper.component.css']
})
export class ImgCropperComponent implements OnInit, AfterViewInit {

  @Input() srcImg: string = null;
  @Output() onCropDone: EventEmitter<Blob> = new EventEmitter<Blob>();
  @Output() onCropCancelled: EventEmitter<any> = new EventEmitter<any>();
  private cropper: Croppie = null;

  constructor() { }

  ngOnInit(): void {
  }

  ngAfterViewInit(): void {

    const el = document.getElementById('img-cropper');
    this.cropper = new Croppie(el, {
      viewport: { width: 350, height: 250 },
      boundary: { width: 400, height: 320 },
      enableZoom: true,
      showZoomer: true,
      enableOrientation: true,
    });
    this.cropper.bind({
      url: this.srcImg,
      orientation: 1
    });

  }

  public onListo(): void {
    if (this.srcImg === null) return;
    this.cropper.result({ type: 'blob' }).then((blob) => {
      this.onCropDone.emit(blob);
    });
  }

  public onCancel(): void {
    if (this.srcImg === null) return;
    this.onCropCancelled.emit();
  }

}
