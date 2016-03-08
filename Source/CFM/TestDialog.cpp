#include <CFM/TestDialog.hpp>

namespace CFM
{

	TestDialog::TestDialog(QWidget* parent)
		:QDialog(parent)
	{
		setupUi(this);
	}

	void TestDialog::dummySlot()
	{

	}
}

