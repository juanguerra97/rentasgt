import { Component, Inject, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { API_BASE_URL, ProductDto, ProductsClient } from '../../rentasgt-api';
import { BsModalService } from 'ngx-bootstrap';
import { ConfirmationModalComponent } from '../../app-common/confirmation-modal/confirmation-modal.component';

@Component({
  selector: 'app-detail-product',
  templateUrl: './detail-product.component.html',
  styleUrls: ['./detail-product.component.css']
})
export class DetailProductComponent implements OnInit {

  public product: ProductDto = null;
  public loadingProduct = false;

  responsiveOptions: any[] = [
    {
      breakpoint: '1024px',
      numVisible: 5
    },
    {
      breakpoint: '768px',
      numVisible: 3
    },
    {
      breakpoint: '560px',
      numVisible: 1
    }
  ];

  constructor(
    private productsClient: ProductsClient,
    private route: ActivatedRoute,
    private router: Router,
    private bsModalService: BsModalService,
    @Inject(API_BASE_URL) public baseUrl?: string,
  ) { }

  ngOnInit(): void {
    this.route.paramMap.subscribe(params => {
      const productId = Number.parseInt(params.get('id'));
      if (!productId) {
        this.router.navigate(['/propietario/productos']);
      } else {
        this.loadProduct(productId);
      }
    });
  }

  private loadProduct(productId: number): void {
    this.loadingProduct = true;
    this.productsClient.getById(productId).subscribe((res) => {
      this.product = res;
      this.loadingProduct = false;
    }, error => {
      this.loadingProduct = false;
      console.error(error);
      this.router.navigate(['/propietario/productos']);
    });
  }

  public showDeleteConfirmationModal(): void {
    const modal = this.bsModalService.show(ConfirmationModalComponent);
    (<ConfirmationModalComponent>modal.content).showConfirmationModal(
      'Eliminación',
      '¿Estás seguro que quieres eliminar este artículo?'
    );

    (<ConfirmationModalComponent>modal.content).onClose.subscribe(result => {
      if (result === true) {
        this.productsClient.delete(this.product.id)
          .subscribe(async (res) => {
            await this.router.navigate(['/propietario/productos']);
          }, console.error);

      }
    });
  }

}
