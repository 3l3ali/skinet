import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { PaginHeaderComponent } from './components/pagin-header/pagin-header.component';

@NgModule({
  declarations: [PaginHeaderComponent],
  imports: [
    CommonModule,
    PaginationModule.forRoot()
  ],
  exports: [
    PaginationModule,
    PaginHeaderComponent
  ]
})
export class SharedModule { }
