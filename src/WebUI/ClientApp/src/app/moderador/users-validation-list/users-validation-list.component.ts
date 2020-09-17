import { Component, OnInit } from '@angular/core';
import { UserProfileDto, UserProfileStatus, UsersClient } from '../../rentasgt-api';
import { PageInfo } from '../../models/PageInfo';
import { FormControl, Validators } from '@angular/forms';

@Component({
  selector: 'app-users-validation-list',
  templateUrl: './users-validation-list.component.html',
  styleUrls: ['./users-validation-list.component.css']
})
export class UsersValidationListComponent implements OnInit {

  DEFAULT_PAGE_NUMBER =  1;
  PAGE_SIZE = 15;

  public cuiFormControl = new FormControl('', [Validators.required]);
  public addressFormControl = new FormControl('', [Validators.required]);

  public pageInfo: PageInfo = new PageInfo(0, 0, 0, 0);
  public users: UserProfileDto[] = [];
  public loadingUsers: boolean = false;
  public selectedUser: UserProfileDto = null;

  public acceptingDpi: boolean = false;
  public acceptingAddress: boolean = false;

  constructor(
    private usersClient: UsersClient,
  ) { }

  ngOnInit(): void {
    this.loadUsers();
  }

  public loadUsers(pageNumber = this.DEFAULT_PAGE_NUMBER, pageSize = this.PAGE_SIZE): void {
    this.loadingUsers = true;
    this.usersClient.getPendingApprovalProfiles(pageNumber, pageSize)
      .subscribe((res) => {
        this.users = res.items;
        this.pageInfo = new PageInfo(res.currentPage, res.totalPages, res.pageSize, res.totalCount);
        this.loadingUsers = false;
      }, error => {
        console.error(error);
        this.loadingUsers = false;
      });
  }

  public onAcceptDpi(): void {
    this.acceptingDpi = true;
    this.usersClient.approveDpi(this.selectedUser.id)
      .subscribe((res) => {
        this.selectedUser.validatedDpi = true;
        if (this.selectedUser.validatedAddress) {
          this.selectedUser.profileStatus = UserProfileStatus.Active;
        }
        this.acceptingDpi = false;
      }, error => {
          console.error(error);
          this.acceptingDpi = false;
      });
  }

  public onAcceptAddress(): void {
    this.acceptingAddress = true;
    this.usersClient.approveAddress(this.selectedUser.id)
      .subscribe((res) => {
        this.selectedUser.validatedAddress = true;
        if (this.selectedUser.validatedDpi) {
          this.selectedUser.profileStatus = UserProfileStatus.Active;
        }
        this.acceptingAddress = false;
      }, error => {
          console.error(error);
          this.acceptingAddress = false;
      });
  }

  public onUserSelected($event): void {
    this.cuiFormControl.reset(this.selectedUser.cui);
    this.addressFormControl.reset(this.selectedUser.address);
  }

  public clearSelectedUser(): void {
    this.selectedUser = null;
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
