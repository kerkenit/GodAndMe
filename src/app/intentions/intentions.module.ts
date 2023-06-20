import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { IonicModule } from '@ionic/angular';
import { FormsModule } from '@angular/forms';

import { IntentionsPage } from './intentions.page';
import { IntentionsPageRoutingModule } from './intentions-routing.module';
import { IntentionComponentModule } from '../intention/intention.module';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    IonicModule,
    IntentionComponentModule,
    IntentionsPageRoutingModule
  ],
  declarations: [IntentionsPage]
})
export class IntentionsPageModule {}
