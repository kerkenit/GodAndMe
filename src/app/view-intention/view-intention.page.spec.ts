import { ComponentFixture, TestBed } from '@angular/core/testing';
import { IonicModule } from '@ionic/angular';
import { RouterModule } from '@angular/router';

import { ViewIntentionPageRoutingModule } from './view-intention-routing.module';
import { ViewIntentionPage } from './view-intention.page';

describe('ViewIntentionPage', () => {
  let component: ViewIntentionPage;
  let fixture: ComponentFixture<ViewIntentionPage>;

  beforeEach(async () => {
    TestBed.configureTestingModule({
      declarations: [ViewIntentionPage],
      imports: [IonicModule.forRoot(), ViewIntentionPageRoutingModule, RouterModule.forRoot([])]
    }).compileComponents();

    fixture = TestBed.createComponent(ViewIntentionPage);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
