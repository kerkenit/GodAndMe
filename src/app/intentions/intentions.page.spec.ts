import { ComponentFixture, TestBed } from '@angular/core/testing';
import { RouterModule } from '@angular/router';
import { IonicModule } from '@ionic/angular';

import { IntentionComponentModule } from '../intention/intention.module';

import { IntentionsPage } from './intentions.page';

describe('IntentionsPage', () => {
  let component: IntentionsPage;
  let fixture: ComponentFixture<IntentionsPage>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [IntentionsPage],
      imports: [IonicModule.forRoot(), IntentionComponentModule, RouterModule.forRoot([])]
    }).compileComponents();

    fixture = TestBed.createComponent(IntentionsPage);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
