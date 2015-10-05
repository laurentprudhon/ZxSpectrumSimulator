# gmake

NAME = zxtests
VERSION = 3
PKG := $(NAME)-$(VERSION)

tmp=tmp.tap
asm=pasmo --alocal
progs=btime stime minfo ulatest3

all: $(addsuffix .tap,$(progs))

%.tap: %.bas
	$(asm) --tap $*.asm $(tmp)
	mktap -b $* 9000 <$*.bas >$@
	cat $(tmp) >>$@

common_asm = instint.asm delay.asm frametime.asm alignint.asm

btime.tap: btime.bas btime.asm $(common_asm) getvar.asm
stime.tap: stime.bas stime.asm $(common_asm) getvar.asm
minfo.tap: minfo.bas minfo.asm $(common_asm) print.asm
ulatest3.tap: ulatest3.bas ulatest3.asm $(common_asm) getvar.asm setvar.asm

FILES := Makefile README $(addsuffix .asm,$(progs)) $(addsuffix .bas,$(progs)) \
	$(common_asm) getvar.asm setvar.asm print.asm

dist:
	ln -s . $(PKG)
	tar czf $(PKG).tar.gz --group=root --owner=root $(addprefix $(PKG)/, $(FILES)); \
	rm $(PKG)
