import 'package:blood_bank_app/screens/home_screen.dart';
import 'package:blood_bank_app/screens/login_screen.dart';
import 'package:blood_bank_app/screens/new_donor_screen.dart';
import 'package:flutter/material.dart';

void main() {
  /// Hide the debug banner
  WidgetsApp.debugAllowBannerOverride = false;
  runApp(MyApp());
}

class MyApp extends StatelessWidget {
  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      restorationScopeId: 'app',
      title: 'Blood Bank 2',
      theme: ThemeData.light(),
      darkTheme: ThemeData.dark(),
      initialRoute: LoginScreen.id,
      routes: {
        HomeScreen.id: (context) => HomeScreen(),
        LoginScreen.id: (context) => LoginScreen(),
        NewDonorScreen.id: (context) => NewDonorScreen(),
      },
    );
  }
}
