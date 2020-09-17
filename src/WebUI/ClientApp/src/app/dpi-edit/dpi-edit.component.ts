import {Component, Inject, OnInit, ViewChild} from '@angular/core';
import {FormControl, Validators} from '@angular/forms';
import * as Croppie from 'croppie';
import {UserProfileDto, UsersClient} from '../rentasgt-api';
import {MAT_DIALOG_DATA, MatDialogRef} from '@angular/material/dialog';
import {imgBlobToBase64} from '../utils';
import {Img} from '../models/Img';
import {CropperComponent} from 'angular-cropperjs';

@Component({
  selector: 'app-dpi-edit',
  templateUrl: './dpi-edit.component.html',
  styleUrls: ['./dpi-edit.component.css']
})
export class DpiEditComponent implements OnInit {

  @ViewChild('dpiCropper') public dpiCropperView: CropperComponent;
  @ViewChild('userCropper') public userCropperView: CropperComponent;

  cuiNameControl = new FormControl('', [Validators.required]);

  public dpiCropperConf = {
    viewMode: 1,
    initialAspectRatio: 1.78,
    aspectRatio: 1.78,
    rotatable: false,
  };
  public userCropperConf = {
    viewMode: 1,
    initialAspectRatio: 1,
    aspectRatio: 1,
    rotatable: false
  };
  public dpiImg: Img = null;
  public userImg: Img = null;

  constructor(
    public usersClient: UsersClient,
    public dialogRef: MatDialogRef<DpiEditComponent>,
    @Inject(MAT_DIALOG_DATA) public user: UserProfileDto,
  ) { }

  ngOnInit(): void {
    this.cuiNameControl.reset(this.user.cui);
  }

  public async onDpiImgFileChange(event): Promise<any> {

    event.preventDefault();
    const fileElement = event.target;
    if (fileElement.files.length > 0) {
      const dpiImgFile = fileElement.files[0];
      try {
        const orig = await imgBlobToBase64(dpiImgFile);

        this.dpiImg = {
          file: dpiImgFile,
          origContent: orig,
          imgCropped: null,
          contentCropped: null
        };
      } catch (error) {
        console.log(error);
      }
    }
  }

  public async onUserImgFileChange(event): Promise<any> {

    event.preventDefault();
    const fileElement = event.target;
    if (fileElement.files.length > 0) {
      const userImgFile = fileElement.files[0];
      try {
        const orig = await imgBlobToBase64(userImgFile);
        this.userImg = {
          file: userImgFile,
          origContent: orig,
          imgCropped: null,
          contentCropped: null
        };
      } catch (error) {
        console.log(error);
      }
    }
  }

  public async onUpdateDpi(): Promise<any> {

    let dpiPic;
    if (this.dpiImg) {
      const dpiBlob = await this.getDpiBlob();
      dpiPic = {
        data: dpiBlob,
        fileName: `${this.user.email}_DPI.png`
      };
    }

    let userPic;
    if (this.userImg) {
      const userBlob = await this.getUserBlob();
      userPic = {
        data: userBlob,
        fileName: `${this.user.email}_USER.png`,
      };
    }

    this.usersClient.updateDpi(this.user.id, this.user.id, this.cuiNameControl.value, dpiPic, userPic)
      .subscribe((res) => {
        this.dialogRef.close(true);
      }, console.error);
  }

  public getDpiBlob(): Promise<Blob> {
    return new Promise<Blob>((resolve, reject) => {
      const canvas = this.dpiCropperView.cropper.getCroppedCanvas();
      return canvas.toBlob(resolve);
    });
  }

  public getUserBlob(): Promise<Blob> {

    return new Promise<Blob>((resolve, reject) => {
      const canvas = this.userCropperView.cropper.getCroppedCanvas();
      return canvas.toBlob(resolve);
    });
  }

}
