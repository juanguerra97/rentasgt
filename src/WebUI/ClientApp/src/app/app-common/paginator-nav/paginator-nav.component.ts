import { Component, OnInit, EventEmitter, Input, Output } from '@angular/core';
import {PageInfo} from '../../models/PageInfo';

@Component({
  selector: 'app-paginator-nav',
  templateUrl: './paginator-nav.component.html',
  styleUrls: ['./paginator-nav.component.css']
})
export class PaginatorNavComponent implements OnInit {

  @Input() pageInfo: PageInfo = null;
  @Output() clickPage: EventEmitter<number> = new EventEmitter<number>();

  constructor() { }

  ngOnInit(): void {

  }

  public previous(): void {
    if (this.pageInfo == null) return;
    const firstAvailablePage = this.pageInfo.pagesSpace[0];
    if (firstAvailablePage === 1) {
      return;
    }
    let previous = firstAvailablePage - (Math.floor(this.pageInfo.PAGINATOR_SIZE / 2));
    if (previous < 1) {
      previous = 1;
    }
    this.clickPage.emit(previous);
  }

  public next(): void {
    if (this.pageInfo == null) return;
    const lastAvailablePage = this.pageInfo.pagesSpace[this.pageInfo.pagesSpace.length - 1];
    if (lastAvailablePage === this.pageInfo.totalPages) {
      return;
    }
    let next = lastAvailablePage + Math.floor(this.pageInfo.PAGINATOR_SIZE / 2);
    console.log(lastAvailablePage);
    if (next > this.pageInfo.totalPages) {
      next = this.pageInfo.totalPages;
    }
    this.clickPage.emit(next);
  }

}
