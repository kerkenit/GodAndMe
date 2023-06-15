import { TestBed } from '@angular/core/testing';

import { PrayersService } from './prayers.service';

describe('PrayersService', () => {
  let service: PrayersService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(PrayersService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
