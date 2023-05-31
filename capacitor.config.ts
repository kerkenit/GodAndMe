import { CapacitorConfig } from '@capacitor/cli';

const config: CapacitorConfig = {
  appId: 'nl.kerkenit.GodAndMe',
  appName: 'GodAndMe',
  webDir: 'dist',
  server: {
    androidScheme: 'https'
  }
};

export default config;
