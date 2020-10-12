import { Component, OnInit } from '@angular/core';
import { UserProfileDto, UsersClient } from '../rentasgt-api';
import { MatDialog } from '@angular/material/dialog';
import { ProfileEditComponent } from '../profile-edit/profile-edit.component';
import { AddressEditComponent } from '../address-edit/address-edit.component';
import { DpiEditComponent } from '../dpi-edit/dpi-edit.component';

@Component({
  selector: 'app-user-profile',
  templateUrl: './user-profile.component.html',
  styleUrls: ['./user-profile.component.css']
})
export class UserProfileComponent implements OnInit {

  public PROFILE_STATUS_INCOMPLETE = 0;
  public PROFILE_STATUS_WAITING_FOR_APPROVAL = 1;
  public PROFILE_STATUS_REJECTED = 2;
  public PROFILE_STATUS_ACTIVE = 3;
  public PROFILE_STATUS_INACTIVE = 4;

  public PROFILE_STATUS_LABELS: string[] = [];
  public PROFILE_STATUS_CLASSES: string[] = [];

  public user: UserProfileDto = null;
  public loadingProfile: boolean = true;

  constructor(
    private usersClient: UsersClient,
    public dialog: MatDialog,
  ) { }

  ngOnInit(): void {
    this.loadUserProfile();
    this.PROFILE_STATUS_LABELS[this.PROFILE_STATUS_INCOMPLETE] = 'Aún no has completado la información de tu perfil.';
    this.PROFILE_STATUS_LABELS[this.PROFILE_STATUS_WAITING_FOR_APPROVAL] = 'Tu cuenta está en proceso de aprobación';
    this.PROFILE_STATUS_LABELS[this.PROFILE_STATUS_REJECTED] = 'Tu cuenta ha sido rechazada.';
    this.PROFILE_STATUS_LABELS[this.PROFILE_STATUS_ACTIVE] = 'Cuenta activa.';

    this.PROFILE_STATUS_CLASSES[this.PROFILE_STATUS_INCOMPLETE] = 'text-warning';
    this.PROFILE_STATUS_CLASSES[this.PROFILE_STATUS_WAITING_FOR_APPROVAL] = 'text-warning';
    this.PROFILE_STATUS_CLASSES[this.PROFILE_STATUS_REJECTED] = 'text-danger';
    this.PROFILE_STATUS_CLASSES[this.PROFILE_STATUS_ACTIVE] = 'text-success';
  }

  private loadUserProfile(): void {
    this.loadingProfile = true;
    this.usersClient.getUserProfile()
      .subscribe((res) => {
        this.user = res;
        console.log(this.user.validatedDpi);
        console.log(this.user.profileStatus);
        this.loadingProfile = false;
      }, error => {
        console.error(error);
        this.loadingProfile = false;
      });
  }

  public openEditProfileDialog(): void {
    const dialogRef = this.dialog.open(ProfileEditComponent, {
      width: '350px',
      data: this.user
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.loadUserProfile();
      }
    });
  }

  public openEditDpiDialog(): void {
    const dialogRef = this.dialog.open(DpiEditComponent, {
      width: '350px',
      data: this.user
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.loadUserProfile();
      }
    });
  }

  public openEditAddressDialog(): void {
    const dialogRef = this.dialog.open(AddressEditComponent, {
      width: '350px',
      data: this.user
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.loadUserProfile();
      }
    });
  }

  public imgLoaded($event) {
    const img: HTMLImageElement = $event.target;
    const ratio = img.width / img.height;
    if (ratio >= 1.0) {
      img.parentElement.classList.remove('img-wrapper-h');
      img.parentElement.classList.add('img-wrapper-w');
    } else {
      img.parentElement.classList.remove('img-wrapper-w');
      img.parentElement.classList.add('img-wrapper-h');
    }
  }

}
