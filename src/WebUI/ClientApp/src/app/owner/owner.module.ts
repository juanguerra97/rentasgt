import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { AppCommonModule } from '../app-common/app-common.module';
import { ProductsComponent } from './products/products.component';
import { NewProductComponent } from './new-product/new-product.component';

@NgModule({
  declarations: [ProductsComponent, NewProductComponent],
  imports: [
    AppCommonModule,
    RouterModule.forChild([
      {
        path: '',
        redirectTo: 'productos',
        pathMatch: 'full',
      },
      {
        path: 'productos',
        component: ProductsComponent
      },
      {
        path: 'productos/nuevo',
        component: NewProductComponent,
      }
    ]),
  ]
})
export class OwnerModule { }
