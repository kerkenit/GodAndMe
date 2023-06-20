import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { ViewIntentionPage } from './view-intention.page';

const routes: Routes = [
  {
    path: '',
    component: ViewIntentionPage
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class ViewIntentionPageRoutingModule {}
