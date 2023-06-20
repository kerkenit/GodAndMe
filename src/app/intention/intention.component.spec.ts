import { ComponentFixture, TestBed } from '@angular/core/testing';
import { RouterModule } from '@angular/router';
import { IonicModule } from '@ionic/angular';

import { IntentionComponent } from './intention.component';

describe('IntentionComponent', () => {
  let component: IntentionComponent;
  let fixture: ComponentFixture<IntentionComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [IntentionComponent],
      imports: [IonicModule.forRoot(), RouterModule.forRoot([])]
    }).compileComponents();

    fixture = TestBed.createComponent(IntentionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
