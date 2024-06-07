using System;
using Skyline.DataMiner.Analytics.GenericInterface;

[GQIMetaData(Name = "Date comparer")]
public class DateComparerOperator : IGQIInputArguments, IGQIColumnOperator, IGQIRowOperator
{
    private readonly GQIColumnDropdownArgument _startColumnArg;
    private readonly GQIColumnDropdownArgument _endColumnArg;
    private readonly GQIStringColumn _comparedColumn;

    private GQIColumn _startColumn;
    private GQIColumn _endColumn;

    public DateComparerOperator()
    {
        _startColumnArg = new GQIColumnDropdownArgument("Start")
        {
            IsRequired = true,
            Types = new GQIColumnType[] { GQIColumnType.DateTime },
        };
        _endColumnArg = new GQIColumnDropdownArgument("End")
        {
            IsRequired = true,
            Types = new GQIColumnType[] { GQIColumnType.DateTime },
        };
        _comparedColumn = new GQIStringColumn("Compared Times");
    }

    public GQIArgument[] GetInputArguments()
    {
        return new GQIArgument[] { _startColumnArg, _endColumnArg };
    }

    public OnArgumentsProcessedOutputArgs OnArgumentsProcessed(OnArgumentsProcessedInputArgs args)
    {
        _startColumn = args.GetArgumentValue(_startColumnArg);
        _endColumn = args.GetArgumentValue(_endColumnArg);
        return default;
    }

    public void HandleColumns(GQIEditableHeader header)
    {
        header.AddColumns(_comparedColumn);
    }

    public void HandleRow(GQIEditableRow row)
    {
        var comparedValue = string.Empty;
        var start = row.GetValue<DateTime>(_startColumn);
        var end = row.GetValue<DateTime>(_endColumn);

        if (start <= DateTime.UtcNow && DateTime.UtcNow <= end)
            comparedValue = "Ongoing";
        else if (DateTime.UtcNow < start)
            comparedValue = "Future";
        else
            comparedValue = "Past";

        row.SetValue(_comparedColumn, comparedValue);
    }
}