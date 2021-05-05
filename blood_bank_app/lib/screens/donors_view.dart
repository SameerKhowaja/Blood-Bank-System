import 'package:flutter/material.dart';

class DonorsView extends StatelessWidget {
  @override
  Widget build(BuildContext context) {
    return RefreshIndicator(
      onRefresh: () => Future.value(),
      child: ListView.separated(
        itemBuilder: (context, idx) => ListTile(
          title: Text('Donor $idx'),
        ),
        separatorBuilder: (_, __) => const Divider(),
        itemCount: 50,
      ),
    );
  }
}
