#include <QtWidgets/QApplication>
#include <CFM/TestDialog.hpp>

int main(int argc, char *argv[])
{
	QApplication a(argc, argv);
	CFM::TestDialog td;
	td.show();
	return a.exec();
}
