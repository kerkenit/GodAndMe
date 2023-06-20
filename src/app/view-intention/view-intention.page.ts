import { CommonModule } from '@angular/common';
import { Component, inject, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { IonicModule, Platform } from '@ionic/angular';
import { IntentionsService, Intention } from '../services/intentions.service';

@Component({
  selector: 'app-view-intention',
  templateUrl: './view-intention.page.html',
  styleUrls: ['./view-intention.page.scss'],
})
export class ViewIntentionPage implements OnInit {
  public intention!: Intention;
  private data = inject(IntentionsService);
  private activatedRoute = inject(ActivatedRoute);
  private platform = inject(Platform);

  constructor() {}

  ngOnInit() {
    const id = this.activatedRoute.snapshot.paramMap.get('id') as string;
    this.intention = this.data.getIntentionById(parseInt(id, 10));
  }

  getBackButtonText() {
    const isIos = this.platform.is('ios')
    return isIos ? 'Intentions' : '';
  }
}
