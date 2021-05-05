import 'package:flutter/material.dart';

class RecipientsView extends StatelessWidget {
  @override
  Widget build(BuildContext context) {
    return RefreshIndicator(
      onRefresh: () => Future.value(),
      child: ListView.separated(
        itemBuilder: (context, idx) => ListTile(
          title: Text('Recipient $idx'),
        ),
        separatorBuilder: (_, __) => const Divider(),
        itemCount: 50,
      ),
    );
  }
}
