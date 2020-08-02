
export class PageInfo {

  public PAGINATOR_SIZE = 5;
  public pagesSpace: number[] = [];

  constructor(
    public currentPage: number,
    public totalPages: number,
    public pageSize: number,
    public totalCount: number
  ) {
    this.createPagesSpace();
  }

  private createPagesSpace(): void {
    this.pagesSpace = [];
    let min = this.currentPage - (Math.floor(this.PAGINATOR_SIZE / 2));
    let max = this.currentPage + (Math.floor(this.PAGINATOR_SIZE / 2));
    if (min < 1) {
      if (max < this.totalPages) {
        max += (Math.abs(min - 1));
      }
      min = 1;
    }

    if (max > this.totalPages) {
      if (min > 1) {
        min -= (Math.abs(max - this.totalPages));
        if (min < 1) {
          min = 1;
        }
      }
      max = this.totalPages;
    }

    this.pagesSpace = [];
    for (let page = min; page <= max; ++page) {
      this.pagesSpace.push(page);
    }

  }

}
