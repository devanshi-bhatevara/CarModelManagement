import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AddCarModelComponent } from './components/add-car-model/add-car-model.component';
import { GetAllComponent } from './components/get-all/get-all.component';
import { UpdateCarModelComponent } from './components/update-car-model/update-car-model.component';

const routes: Routes = [
  {path:'',redirectTo:'getall',pathMatch:'full'},
  {path:'addCarModel', component: AddCarModelComponent},
  {path:'getall', component: GetAllComponent},
  { path: 'update/:id', component: UpdateCarModelComponent },

];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
