import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { IntentionsPage } from './intentions.page';

const routes: Routes = [
  {
    path: '',
    component: IntentionsPage
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class IntentionsPageRoutingModule {}
