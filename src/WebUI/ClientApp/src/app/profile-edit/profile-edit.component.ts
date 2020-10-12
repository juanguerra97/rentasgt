import { Component, Inject, OnInit, ViewChild } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { FormControl, Validators } from '@angular/forms';
import { CropperComponent } from 'angular-cropperjs';
import {SendPhoneNumberVerificationCode, UserProfileDto, UsersClient, ValidateUserPhoneNumberCommand} from '../rentasgt-api';
import { imgBlobToBase64 } from '../utils';
import { Img } from '../models/Img';

@Component({
  selector: 'app-profile-edit',
  templateUrl: './profile-edit.component.html',
  styleUrls: ['./profile-edit.component.css']
})
export class ProfileEditComponent implements OnInit {

  @ViewChild('profileCropper') public profileCropperView: CropperComponent;

  phoneNumberControl = new FormControl({value: '', disabled: false}, [
    Validators.required,
    Validators.pattern(`^([0-9]{8})$`)
  ]);
  verificationCodeControl = new FormControl('', [
    Validators.required,
    Validators.pattern(`^([0-9]{6})$`)
  ]);
  public saving: boolean = false;
  public sendingVerificationCode: boolean = false;
  public validatingCode: boolean = false;
  public invalidCode: boolean = false;
  public sentPhoneNumber: boolean = false;

  public profileCropperConf = {
    viewMode: 1,
    initialAspectRatio: 1,
    aspectRatio: 1,
    rotatable: false,
  };

  public profileImg: Img = null;

  constructor(
    public usersClient: UsersClient,
    public dialogRef: MatDialogRef<ProfileEditComponent>,
    @Inject(MAT_DIALOG_DATA) public user: UserProfileDto,
  ) { }

  ngOnInit(): void {
    this.phoneNumberControl.reset(this.user.phoneNumber);
  }

  public async onImgFileChange(event): Promise<any> {

    event.preventDefault();
    const fileElement = event.target;
    if (fileElement.files.length > 0) {
      const imgFile = fileElement.files[0];
      try {
        const orig = await imgBlobToBase64(imgFile);
        this.profileImg = {
          file: imgFile,
          origContent: orig,
          contentCropped: null,
          imgCropped: null
        };
      } catch (error) {
        console.log(error);
      }
    }
  }

  public async onUpdateProfilePic(): Promise<any> {
    this.saving = true;
      this.usersClient.updateProfileInfo(this.user.id, this.user.id, undefined, undefined,
        {data: await this.getProfileBlob(), fileName: this.user.firstName + '_' + this.user.lastName + '.png'})
        .subscribe((res) => {
          this.saving = false;
          this.dialogRef.close(true);
        }, error => {
            console.error(error);
            this.saving = false;
        });
  }

  public getProfileBlob(): Promise<Blob> {
    return new Promise<Blob>((resolve, reject) => {
      const canvas = this.profileCropperView.cropper.getCroppedCanvas();
      canvas.toBlob(resolve);
    });
  }

  public onUpdatePhoneNumber(): void {
    this.phoneNumberControl.disable();
    this.invalidCode = false;
    this.sentPhoneNumber = false;
    this.sendingVerificationCode = true;
    this.usersClient.sendPhoneVerificationCode(new SendPhoneNumberVerificationCode({
      phoneNumber: this.phoneNumberControl.value
    }))
      .subscribe((res) => {
        this.sentPhoneNumber = true;
        this.sendingVerificationCode = false;
      }, error => {
        this.phoneNumberControl.enable();
        console.error(error);
        this.sendingVerificationCode = false;
      });
  }

  public onValidateVerificationCode(): void {
    this.validatingCode = true;
    this.invalidCode = false;
    this.usersClient.validatePhoneVerificationCode(new ValidateUserPhoneNumberCommand({
      verificationCode: this.verificationCodeControl.value,
      phoneNumber: this.phoneNumberControl.value
    }))
      .subscribe((res) => {
        this.phoneNumberControl.enable();
        this.invalidCode = !res;
        if (res) {
          this.user.phoneNumberConfirmed = true;
          this.dialogRef.close(res);
        }
        this.validatingCode = false;
      }, error => {
        this.invalidCode = true;
        this.validatingCode = false;
        console.error(error);
      });
  }

  public onCancelPhoneNumberUpdate(): void {
    this.phoneNumberControl.enable();
    if (this.user.phoneNumber) {
      this.phoneNumberControl.reset(this.user.phoneNumber);
    } else {
      this.phoneNumberControl.reset();
    }
    this.verificationCodeControl.reset();
    this.invalidCode = false;
    this.sentPhoneNumber = false;
  }

}
