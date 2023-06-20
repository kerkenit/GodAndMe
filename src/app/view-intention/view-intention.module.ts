import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ViewIntentionPage } from './view-intention.page';

import { IonicModule } from '@ionic/angular';

import { ViewIntentionPageRoutingModule } from './view-intention-routing.module';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    IonicModule,
    ViewIntentionPageRoutingModule
  ],
  declarations: [ViewIntentionPage]
})
export class ViewIntentionPageModule {}
