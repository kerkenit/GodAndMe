import { ChangeDetectionStrategy, Component, inject, Input } from '@angular/core';
import { Platform } from '@ionic/angular';
import { Intention } from '../services/intentions.service';

@Component({
  selector: 'app-intention',
  templateUrl: './intention.component.html',
  styleUrls: ['./intention.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class IntentionComponent {
  private platform = inject(Platform);
  @Input() intention?: Intention;
  isIos() {
    return this.platform.is('ios')
  }
}
