import 'package:blood_bank_app/screens/dashboard_view.dart';
import 'package:blood_bank_app/screens/donors_view.dart';
import 'package:blood_bank_app/screens/login_screen.dart';
import 'package:blood_bank_app/screens/new_donor_screen.dart';
import 'package:blood_bank_app/screens/recipients_view.dart';
import 'package:flutter/material.dart';

class HomeScreen extends StatefulWidget {
  static const id = 'home';

  @override
  _HomeScreenState createState() => _HomeScreenState();
}

class _HomeScreenState extends State<HomeScreen> with RestorationMixin {
  /// Index of the currently visible tab
  final _currentIndex = RestorableInt(0);

  final _bottomNavBarItems = <BottomNavigationBarItem>[
    BottomNavigationBarItem(
      icon: const Icon(Icons.home),
      label: 'Home',
    ),
    BottomNavigationBarItem(
      icon: const Icon(Icons.people),
      label: 'Donors',
    ),
    BottomNavigationBarItem(
      icon: const Icon(Icons.people_outline),
      label: 'Recipients',
    ),
  ];

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: const Text('Blood Bank 2'),
        actions: [
          IconButton(
            icon: const Icon(Icons.logout),
            tooltip: 'Logout',
            onPressed: () {
              Navigator.restorablePushReplacementNamed(
                context,
                LoginScreen.id,
              );
            },
          ),
        ],
      ),
      body: IndexedStack(
        index: _currentIndex.value,
        children: [
          DashboardView(),
          DonorsView(),
          RecipientsView(),
        ],
      ),
      bottomNavigationBar: BottomNavigationBar(
        currentIndex: _currentIndex.value,
        items: _bottomNavBarItems,
        type: BottomNavigationBarType.fixed,
        onTap: (index) {
          if (index != _currentIndex.value) {
            setState(() => _currentIndex.value = index);
          }
        },
      ),
      floatingActionButton: FloatingActionButtonDelegate(
        fabWidgets: <int, Widget>{
          0: FloatingActionButton.extended(
            onPressed: () {
              /// TODO: Open new screen
            },
            icon: const Icon(Icons.add),
            label: const Text('Add Donation'),
          ),
          1: FloatingActionButton.extended(
            onPressed: () {
              Navigator.restorablePushNamed(context, NewDonorScreen.id);
            },
            icon: const Icon(Icons.add),
            label: const Text('New Donor'),
          ),
          2: FloatingActionButton.extended(
            onPressed: () {},
            icon: const Icon(Icons.add),
            label: const Text('New Recipient'),
          ),
        },
        currentIndex: _currentIndex.value,
      ),
    );
  }

  @override
  String get restorationId => 'bottom_nav_bar';

  @override
  void restoreState(RestorationBucket? oldBucket, bool initialRestore) {
    registerForRestoration(_currentIndex, 'bottom_nav_bar_index');
  }
}

class FloatingActionButtonDelegate extends StatelessWidget {
  final Map<int, Widget> fabWidgets;
  final int currentIndex;

  const FloatingActionButtonDelegate({
    Key? key,
    required this.fabWidgets,
    required this.currentIndex,
  }) : super(key: key);

  @override
  Widget build(BuildContext context) {
    if (fabWidgets.containsKey(currentIndex) == false) return Container();
    return fabWidgets[currentIndex]!;
  }
}
