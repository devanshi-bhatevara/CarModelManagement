import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { AddCarModelComponent } from './components/add-car-model/add-car-model.component';
import { FormsModule } from '@angular/forms';
import { GetAllComponent } from './components/get-all/get-all.component';
import { AlphanumericDirective } from './directives/alphanumeric.directive';
import { UpdateCarModelComponent } from './components/update-car-model/update-car-model.component';

@NgModule({
  declarations: [
    AppComponent,
    AddCarModelComponent,
    GetAllComponent,
    AlphanumericDirective,
    UpdateCarModelComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    FormsModule
  ],
  providers: 
  [],
  bootstrap: [AppComponent]
})
export class AppModule { }
