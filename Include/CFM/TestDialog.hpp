#pragma once
#include <ui_TestDialog.h>
#include <QDialog>

namespace CFM
{
	class TestDialog: public QDialog, public Ui::Dialog
	{
		Q_OBJECT
	public:
		TestDialog(QWidget* parent = 0);

	signals:
		void dummySignal();

	public slots:
		void dummySlot();
	};
}