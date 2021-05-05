import 'package:flutter/material.dart';

class DashboardView extends StatelessWidget {
  final groupNames = <String>['O+', 'O-', 'A+', 'A-', 'B+', 'B-', 'AB+', 'AB-'];
  final groupUnits = <int>[10, 5, 11, 7, 8, 9, 12, 3];

  @override
  Widget build(BuildContext context) {
    return RefreshIndicator(
      onRefresh: () => Future.value(),
      child: GridView.count(
        padding: const EdgeInsets.fromLTRB(8.0, 8.0, 8.0, 56.0),
        physics: const AlwaysScrollableScrollPhysics(),
        crossAxisCount: 2,
        children: List<Widget>.generate(
          groupNames.length,
          (index) => Card(
            child: Row(
              mainAxisAlignment: MainAxisAlignment.spaceEvenly,
              children: [
                Text(groupNames[index],
                    style: Theme.of(context).textTheme.headline4),
                Text(groupUnits[index].toString(),
                    style: Theme.of(context).textTheme.headline4),
              ],
            ),
          ),
        ),
      ),
    );
  }
}
