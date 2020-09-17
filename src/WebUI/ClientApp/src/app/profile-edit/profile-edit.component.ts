import {Component, Inject, OnInit, ViewChild} from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { FormControl, Validators } from '@angular/forms';
import { CropperComponent } from 'angular-cropperjs';
import {UpdatePhoneNumberCommand, UserProfileDto, UsersClient} from '../rentasgt-api';
import { imgBlobToBase64 } from '../utils';
import { Img } from '../models/Img';

@Component({
  selector: 'app-profile-edit',
  templateUrl: './profile-edit.component.html',
  styleUrls: ['./profile-edit.component.css']
})
export class ProfileEditComponent implements OnInit {

  @ViewChild('profileCropper') public profileCropperView: CropperComponent;

  phoneNumberControl = new FormControl('', [
    Validators.required,
    Validators.pattern(`^([0-9]{8})$`)
  ]);

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
      this.usersClient.updateProfileInfo(this.user.id, this.user.id, undefined, undefined,
        {data: await this.getProfileBlob(), fileName: this.user.firstName + '_' + this.user.lastName + '.png'})
        .subscribe((res) => {
          this.dialogRef.close(true);
        }, console.error);
  }

  public getProfileBlob(): Promise<Blob> {
    return new Promise<Blob>((resolve, reject) => {
      const canvas = this.profileCropperView.cropper.getCroppedCanvas();
      canvas.toBlob(resolve);
    });
  }

  public onUpdatePhoneNumber(): void {
    this.usersClient.updatePhoneNumber(this.user.id, new UpdatePhoneNumberCommand({
      userId: this.user.id, phoneNumber: this.phoneNumberControl.value
    }))
      .subscribe((res) => {
        this.dialogRef.close(true);
      }, console.error);
  }

}
