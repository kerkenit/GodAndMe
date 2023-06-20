import { Component, inject } from '@angular/core';
import { RefresherCustomEvent } from '@ionic/angular';
import { IntentionComponent } from '../intention/intention.component';

import { IntentionsService, Intention } from '../services/intentions.service';

@Component({
  selector: 'app-intentions',
  templateUrl: 'intentions.page.html',
  styleUrls: ['intentions.page.scss'],
})
export class IntentionsPage {
  private data = inject(IntentionsService);
  constructor() {}

  refresh(ev: any) {
    setTimeout(() => {
      (ev as RefresherCustomEvent).detail.complete();
    }, 3000);
  }

  getIntentions(): Intention[] {
    return this.data.getIntentions();
  }
}
