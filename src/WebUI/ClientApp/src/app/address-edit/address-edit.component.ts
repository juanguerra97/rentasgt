import { Component, Inject, OnInit } from '@angular/core';
import { FormControl, Validators } from '@angular/forms';
import * as Croppie from 'croppie';
import { UserProfileDto, UsersClient } from '../rentasgt-api';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { imgBlobToBase64 } from '../utils';
import { Img } from '../models/Img';

@Component({
  selector: 'app-address-edit',
  templateUrl: './address-edit.component.html',
  styleUrls: ['./address-edit.component.css']
})
export class AddressEditComponent implements OnInit {

  addressControl = new FormControl('', [Validators.required]);

  public addressImg: Img = null;

  constructor(
    public usersClient: UsersClient,
    public dialogRef: MatDialogRef<AddressEditComponent>,
    @Inject(MAT_DIALOG_DATA) public user: UserProfileDto,
  ) { }

  ngOnInit(): void {
    this.addressControl.reset(this.user.address);
  }

  public async onImgFileChange(event): Promise<any> {

    event.preventDefault();
    const fileElement = event.target;
    if (fileElement.files.length > 0) {
      const imgFile = fileElement.files[0];
      try {
        const orig = await imgBlobToBase64(imgFile);
        this.addressImg = {
          file: imgFile,
          origContent: orig,
          imgCropped: null,
          contentCropped: null
        };

      } catch (error) {
        console.log(error);
      }
    }
  }

  public onUpdateAddress(): void {
    if (this.addressImg) {
        this.usersClient.updateAddress(this.user.id, this.user.id, this.addressControl.value, {data: this.addressImg.file, fileName: this.addressImg.file.name})
          .subscribe((res) => {
            this.dialogRef.close(true);
          }, console.error);
    } else {
      this.usersClient.updateAddress(this.user.id, this.user.id, this.addressControl.value, undefined)
        .subscribe((res) => {
          this.dialogRef.close(true);
        }, console.error);
    }
  }

  public imgLoaded($event) {
    const img: HTMLImageElement = $event.target;
    const ratio = img.width / img.height;
    if (ratio >= 1.0) {
      img.parentElement.classList.remove('image-wrapper-h');
      img.parentElement.classList.add('image-wrapper-w');
    } else {
      img.parentElement.classList.remove('image-wrapper-w');
      img.parentElement.classList.add('image-wrapper-h');
    }
  }
}
