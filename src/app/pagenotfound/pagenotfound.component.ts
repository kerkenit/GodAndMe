import { Component, inject, OnInit } from '@angular/core';
import { IonicModule, Platform } from '@ionic/angular';

@Component({
  selector: 'app-pagenotfound',
  templateUrl: './pagenotfound.component.html',
  styleUrls: ['./pagenotfound.component.scss'],
})
export class PagenotfoundComponent  implements OnInit {

  private platform = inject(Platform);
  constructor() { }

  ngOnInit() {}

  getBackButtonText() {
    const isIos = this.platform.is('ios')
    return isIos ? 'Back' : '';
  }

  getBackButtonDisplay() {
    const isIos = this.platform.is('ios')
    return isIos ? 'block' : 'none';
  }
}
