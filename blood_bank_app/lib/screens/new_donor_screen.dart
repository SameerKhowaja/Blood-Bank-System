import 'package:flutter/cupertino.dart';
import 'package:flutter/material.dart';

class NewDonorScreen extends StatefulWidget {
  static const id = 'new_donor';

  @override
  _NewDonorScreenState createState() => _NewDonorScreenState();
}

class _NewDonorScreenState extends State<NewDonorScreen> {
  final _donorFormKey = GlobalKey<FormState>();

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: const Text('New Donor'),
      ),
      body: AutofillGroup(
        child: Form(
          key: _donorFormKey,
          child: ListView(
            padding: const EdgeInsets.all(24.0),
            children: [
              /// Full name
              TextFormField(
                decoration: const InputDecoration(
                  labelText: 'Full Name',
                  border: OutlineInputBorder(),
                ),
                textInputAction: TextInputAction.next,
              ),

              const SizedBox(height: 8),

              /// Date of birth
              OutlinedButton.icon(
                onPressed: () {
                  showDatePicker(
                    context: context,
                    initialDate: DateTime(2000),
                    firstDate: DateTime(1940),
                    lastDate: DateTime.now().subtract(
                      const Duration(days: 365 * 16),
                    ),
                  );
                },
                icon: const Icon(Icons.calendar_today),
                label: const Text('Date of Birth'),
              ),

              const SizedBox(height: 8),

              /// Gender
              CupertinoSlidingSegmentedControl<String>(
                children: <String, Widget>{
                  'Male': const Text('Male'),
                  'Female': const Text('Female'),
                  'Prefer not to say': const Text('Prefer not to say'),
                },
                onValueChanged: (newValue) {},
              ),

              const SizedBox(height: 8),

              /// BloodGroup
              DropdownButton<String>(
                hint: const Text('Select Blood Group'),
                underline: SizedBox(),
                isExpanded: true,
                items: List<DropdownMenuItem<String>>.generate(
                  8,
                  (index) => DropdownMenuItem(
                    value: index.toString(),
                    child: Text('Group $index'),
                  ),
                ),
                onTap: () {},
                onChanged: (newValue) {},
              ),

              const SizedBox(height: 8),

              /// Last donated
              OutlinedButton.icon(
                onPressed: () {
                  showDatePicker(
                    context: context,
                    initialDate: DateTime(2000),
                    firstDate: DateTime(1940),
                    lastDate: DateTime.now().subtract(
                      const Duration(days: 365 * 16),
                    ),
                  );
                },
                icon: const Icon(Icons.calendar_today),
                label: const Text('Last Donated'),
              ),

              const SizedBox(height: 8),

              /// Phone NUmber
              TextFormField(
                decoration: const InputDecoration(
                  labelText: 'Phone number',
                  border: OutlineInputBorder(),
                ),
                textInputAction: TextInputAction.next,
              ),

              const SizedBox(height: 8),

              /// Email
              TextFormField(
                decoration: const InputDecoration(
                  labelText: 'Email address',
                  border: OutlineInputBorder(),
                ),
                textInputAction: TextInputAction.next,
              ),

              const SizedBox(height: 8),

              /// Address
              TextFormField(
                decoration: const InputDecoration(
                  labelText: 'Address',
                  border: OutlineInputBorder(),
                ),
                keyboardType: TextInputType.multiline,
                textInputAction: TextInputAction.done,
                minLines: 1,
                maxLines: null,
              ),
            ],
          ),
        ),
      ),
      persistentFooterButtons: [
        OutlinedButton(
          onPressed: () {
            Navigator.pop(context);
          },
          child: const Text('Cancel'),
        ),
        ElevatedButton(
          onPressed: () {
            Navigator.pop(context);
          },
          child: const Text('Save'),
        ),
      ],
    );
  }
}
